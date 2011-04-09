#!/bin/bash

cd `dirname $0`

# generate hand-crafted documentation
echo "Generating hand-made documentation..."
cd src
mkdir -p rst-out
for F in *.rst; do
    echo "    $F"
    rst2html --template=../template/rst.html $F > rst-out/${F%.rst}.html
done
cd ..

# generate api documentation
echo "Running doxygen..."
doxygen >/dev/null

# clean up
rm -rf src/rst-out

echo "Done."
