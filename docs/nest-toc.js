#!/usr/bin/env node
// Restructure api/toc.yml so sub-namespaces are nested under their parent namespace.
const fs = require('fs');
const path = require('path');

const tocPath = path.join(__dirname, 'api', 'toc.yml');
const content = fs.readFileSync(tocPath, 'utf8');
const allLines = content.split('\n');

const header = allLines[0]; // ### YamlMime:TableOfContent

// Split into top-level namespace blocks at lines starting with "- uid:"
const blocks = [];
let current = null;
for (let i = 2; i < allLines.length; i++) {
  const line = allLines[i];
  if (/^- uid:/.test(line)) {
    if (current) blocks.push(current);
    current = { uid: line.replace('- uid:', '').trim(), rawLines: [line] };
  } else if (/^\w/.test(line)) {
    // Root-level mapping key (e.g. "memberLayout: SamePage") — attach to current block
    // as a top-level field, not inside items
    if (current) current.rootFields = (current.rootFields || []).concat(line);
  } else if (current) {
    current.rawLines.push(line);
  }
}
if (current) blocks.push(current);

// Trim trailing empty lines
for (const b of blocks) {
  while (b.rawLines.length > 1 && b.rawLines[b.rawLines.length - 1].trim() === '') {
    b.rawLines.pop();
  }
}

const uidMap = {};
for (const b of blocks) uidMap[b.uid] = b;
const allUids = new Set(blocks.map(b => b.uid));

function findParent(uid) {
  const parts = uid.split('.');
  for (let i = parts.length - 1; i > 0; i--) {
    const candidate = parts.slice(0, i).join('.');
    if (allUids.has(candidate)) return candidate;
  }
  return null;
}

for (const b of blocks) b.nsChildren = [];
const roots = [];
for (const b of blocks) {
  const parent = findParent(b.uid);
  if (parent) uidMap[parent].nsChildren.push(b);
  else roots.push(b);
}

function reindent(line, targetIndent) {
  if (line.trim() === '') return '';
  const col = line.search(/\S/);
  return ' '.repeat(targetIndent + col) + line.trimStart();
}

function emit(block, targetIndent) {
  const raw = block.rawLines;
  const itemsLineIdx = raw.findIndex(l => /^\s+items:/.test(l));

  if (block.nsChildren.length === 0 || itemsLineIdx === -1) {
    // No restructuring needed — but still hoist any stray fields after items
    if (itemsLineIdx === -1) {
      return raw.map(l => reindent(l, targetIndent)).join('\n') + '\n';
    }
    // Separate header, extra fields, type items
    const header = raw.slice(0, itemsLineIdx);
    const afterItems = raw.slice(itemsLineIdx + 1);
    const itemsIndentCol = raw[itemsLineIdx].search(/\S/);
    const typeItems = afterItems.filter(l => l.trim() === '' || l.search(/\S/) !== itemsIndentCol || /^\s*-/.test(l));
    const extraFields = afterItems.filter(l => l.trim() !== '' && l.search(/\S/) === itemsIndentCol && !/^\s*-/.test(l));
    const rootFieldLines = (block.rootFields || []).map(l => '  ' + l);
    const reordered = [
      ...header,
      ...extraFields,
      ...rootFieldLines,
      raw[itemsLineIdx],
      ...typeItems,
    ];
    return reordered.map(l => reindent(l, targetIndent)).join('\n') + '\n';
  }

  // Split rawLines into: header, extra fields (like memberLayout), items line, type items
  const headerLines = raw.slice(0, itemsLineIdx);
  const afterItems = raw.slice(itemsLineIdx + 1);
  const itemsIndentCol = raw[itemsLineIdx].search(/\S/);
  const typeItems = afterItems.filter(l => l.trim() === '' || l.search(/\S/) !== itemsIndentCol || /^\s*-/.test(l));
  const extraFields = afterItems.filter(l => l.trim() !== '' && l.search(/\S/) === itemsIndentCol && !/^\s*-/.test(l));

  const itemsLine = raw[itemsLineIdx];
  const nsEmitted = block.nsChildren.flatMap(c => emit(c, targetIndent + 2).split('\n'));
  while (nsEmitted.length && nsEmitted[nsEmitted.length - 1] === '') nsEmitted.pop();

  const rootFieldLines = (block.rootFields || []).map(l => reindent('  ' + l, targetIndent));
  const resultLines = [
    ...headerLines.map(l => reindent(l, targetIndent)),
    ...extraFields.map(l => reindent(l, targetIndent)),
    ...rootFieldLines,
    reindent(itemsLine, targetIndent),
    ...nsEmitted,
    ...typeItems.map(l => reindent(l, targetIndent)),
  ];

  return resultLines.join('\n') + '\n';
}

let output = header + '\nitems:\n';
for (const root of roots) output += emit(root, 0);

fs.writeFileSync(tocPath, output);
console.log(`Nested ${blocks.length} namespaces into ${roots.length} top-level item(s).`);
