#!/usr/bin/env node
// Remove JsonPropertyName attribute annotations from all generated API YAML files.
const fs = require('fs');
const path = require('path');

const apiDir = path.join(__dirname, 'api');
const files = fs.readdirSync(apiDir).filter(f => f.endsWith('.yml'));

let total = 0;
for (const file of files) {
  const filePath = path.join(apiDir, file);
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;

  // Remove [Json*(...)] or [Json*] lines (C# syntax) + the blank line that follows
  content = content.replace(/^ *\[Json\w+(?:\([^\]]*\))?\]\n\n?/gm, '');

  // Remove <Json*(...)> or <Json*> lines (VB syntax) + the blank line that follows
  content = content.replace(/^ *<Json\w+(?:\([^>]*\))?>\n\n?/gm, '');

  // Remove any System.Text.Json.* attribute entry block inside attributes: lists
  content = content.replace(
    /  - type: System\.Text\.Json\.[^\n]+\n(?:    [^\n]*\n)*/g,
    ''
  );

  // Remove now-empty "  attributes:" keys
  content = content.replace(/  attributes:\n(?=\S)/g, '');

  if (content !== original) {
    fs.writeFileSync(filePath, content);
    total++;
  }
}
console.log(`Stripped JsonPropertyName from ${total} file(s).`);
