#!/bin/bash
set -e

cd "$(dirname "$0")"

# Generate metadata
docfx metadata docfx.json

# Strip JsonPropertyName attribute annotations from generated YAML
node strip-json-attributes.js

# Restructure flat namespace TOC into a proper nested hierarchy
node nest-toc.js

# Build site (pass any extra args, e.g. --serve)
docfx build docfx.json "$@"

echo "Documentation generated in _site/"
