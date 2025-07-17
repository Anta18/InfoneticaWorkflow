#!/usr/bin/env bash
set -euo pipefail

# Output file
output="combined.txt"

# Start with an empty output
> "$output"

# Find and process matching files, pruning bin/ and obj/ dirs
find . \
  \( -type d \( -name bin -o -name obj \) -prune \) -o \
  \( -type f \( -name "*.csproj" -o -name "*.proj" -o -name "*.cs" \) -print0 \) | \
while IFS= read -r -d '' file; do
  echo "===== $file =====" >> "$output"
  cat "$file" >> "$output"
  echo >> "$output"
done

echo "All files combined into $output"
