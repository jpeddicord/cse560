#!/bin/bash

cd `dirname $0`

# generate api documentation
echo "Running doxygen..."
doxygen >/dev/null

# generate hand-crafted documentation
echo "Generating hand-made documentation..."
mkdir -p api/html/rst
cd src
for F in *.rst; do
    echo "    $F"
    rst2html $F > ../out/${F%.rst}.html
done

echo "Done."
