#!/bin/bash

cd `dirname $0`

# generate api documentation
doxygen

# generate hand-crafted documentation
mkdir -p out
for F in *.rst; do
    rst2html $F > out/$F.html
done
