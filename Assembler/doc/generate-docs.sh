#!/bin/bash -e

cd `dirname $0`
cd src

# extract DED data
echo "Extracting DED data..."

tmpfile=ded.tmp
echo > $tmpfile

for F in ../../Assembler/*.csv; do
    base=`basename $F`
    module=${base%.csv}
    echo "    $base"
    echo "$module" >> $tmpfile
    for ((i=0; i < ${#module}; i++)); do
        echo -n '~' >> $tmpfile
    done
    echo -e "\n\n.. csv-table::" >> $tmpfile
    echo "   :header: \"Variable Name\", \"Module Defined\", \"Data Type\", \"Local/Global\", \"Purpose\"" >> $tmpfile
    echo -e "   :file: $F\n\n" >> $tmpfile
done

# generate hand-crafted documentation
echo "Generating hand-made documentation..."
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
rm -f src/*.tmp

echo "Done."
