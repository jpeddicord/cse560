#!/usr/bin/env python
# Author: Jacob Peddicord <peddicord.17@osu.edu>

import os
from os.path import join, basename
from subprocess import call, Popen, PIPE
from shutil import rmtree
from docutils.core import publish_file
from docutils.writers import manpage, html4css1


def build():
    """Documentation builder"""
    # set up
    try:
        os.mkdir('tmp')
    except: pass

    # build
    print "Building DED"
    build_ded('../Assembler/', 'tmp/ded.rst')
    convert_rst('tmp/ded.rst', 'tmp/ded.html')
    create_dox_wrapper('tmp/ded.rst', 'tmp/ded.dox')
    print "Building manual"
    build_rst_dir('src', 'tmp')
    print "Running Doxygen"
    doxygen()
    
    # clean up
    rmtree('tmp')

def build_ded(directory, out_filename):
    """Process a template for DEDs."""
    
    def create_ded_rst(csv_filename):
        template = """
{title}
{titlebar}

.. csv-table::
   :header: "Variable Name", "Module Defined", "Data Type", "Local/Global", "Purpose"

{content}

"""
        title = basename(csv_filename).replace('.csv', '')
        content = []
        with open(csv_filename) as f:
            for line in f:
                content.append("   " + line.strip())
        return template.format(title=title, titlebar='='*len(title), filename=csv_filename, content='\n'.join(content))
    
    # write the output
    with open(out_filename, 'w') as out:
        out.write("=======================\nData Element Dictionary\n=======================")
        # read the directory for csv files
        for root, dirs, files in os.walk(directory):
            for fname in files:
                if fname.endswith('.csv'):
                    out.write(create_ded_rst(join(root, fname)))

def build_rst_dir(directory, out_dir):
    """Generate HTML for all RST files in the given directory. Additionally, create dox wrappers."""
    for root, dirs, files in os.walk(directory):
        for fname in files:
            if fname.endswith('.rst'):
                convert_rst(join(root, fname), join(out_dir, fname.replace('.rst', '.html')))
                create_dox_wrapper(join(root, fname), join(out_dir, fname.replace('.rst', '.dox')))

def convert_rst(rst_file, out_file):
    """Use ReST to convert the given file into HTML."""
    publish_file(source_path=rst_file, destination_path=out_file, writer=html4css1.Writer(), settings_overrides={'template': 'template/rst.html'})

def create_dox_wrapper(rst_file, out_file):
    """Create a doxygen wrapper file to embed the given file into."""

    # get the title of the document
    title = "Untitled Document"
    with open(rst_file) as f:
        for line in f:
            if not line.startswith('='):
                title = line.strip()
                break

    # write the template
    with open(out_file, 'w') as f:
        f.write("""/**

\\page {base} {title}
\\htmlinclude {base}.html

*/""".format(base=basename(rst_file).replace('.rst', ''), title=title))

def doxygen():
    """Run Doxygen."""
    Popen("doxygen", shell=True, stdout=PIPE).communicate()

if __name__ == "__main__":
    build()
