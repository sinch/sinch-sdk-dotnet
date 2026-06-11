#!/bin/bash
set -e

cd "$(dirname "$0")"

# Generate metadata
docfx metadata docfx.json

# Build site
docfx build docfx.json

echo "Documentation generated in _site/"
