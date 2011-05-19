#!/usr/bin/env python
# Author: Jacob Peddicord <peddicord.17@osu.edu>

import os
import re
from os.path import join, basename
from subprocess import call, Popen, PIPE
from shutil import rmtree, copytree
from docutils.core import publish_file
from docutils.writers import manpage, html4css1


def build():
    """Documentation builder"""
    # clean up old builds
    try:
        rmtree('tmp')
        rmtree('out')
    except: pass
    try:
        os.makedirs('tmp/tests')
    except: pass

    # build
    print "Building DED"
    build_ded('../', 'tmp/ded.rst')
    convert_rst('tmp/ded.rst', 'tmp/ded.html')
    create_dox_wrapper('tmp/ded.rst', 'tmp/ded.dox')
    print "Running test scripts"
    scripts = build_test_scripts('../Tests/Programs', '../bin/Release/Assembler.exe', 'tmp/tests', 'testfile_', 'tmp/testscript_index.rst')
    for script in scripts:
        convert_rst(join('tmp/tests', script), join('tmp', script.replace('.rst', '.html')))
        create_dox_wrapper(join('tmp/tests', script), join('tmp', script.replace('.rst', '.dox')))
    print "Creating error listing"
    build_error_list('../Resources/errors.txt', 'tmp/errorlist.rst')
    print "Building manual"
    build_rst_dir('src', 'tmp')
    print "Running Doxygen"
    doxygen()
    print "Copying images"
    copytree("src/images", "out/html/images")

def build_ded(directory, out_filename):
    """Process a template for DEDs."""
    
    def create_ded_rst(csv_filename):
        template = """
{title}
{titlebar}

.. csv-table::
   :header: "Variable Name", "Module Defined", "Data Type", "Local/Global", "Purpose", "Range"

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
        out.write("\n\n.. contents::\n\n")
        # read the directory for csv files
        for root, dirs, files in os.walk(directory):
            files.sort()
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

def camelcase_to_underscore(txt):
    """http://stackoverflow.com/questions/1175208/"""
    s1 = re.sub('(.)([A-Z][a-z]+)', r'\1_\2', txt)
    return re.sub('([a-z0-9])([A-Z])', r'\1_\2', s1).lower()

def build_test_scripts(directory, runner, out_dir, prefix, index_file):
    """Copy the test script input, run the script, and copy output to an RST source."""
    names = []
    for root, dirs, files in os.walk(directory):
        files.sort()
        for fname in files:
            if fname.endswith('.txt'):
                out = fname + '\n' + '`'*len(fname) + '\n\n.. contents::'
                # get the info file
                try:
                    with open(join(root, fname.replace('.txt', '.info'))) as f:
                        out += '\n\n' + f.read()
                except:
                    pass
                out += '\n\nInput\n^^^^^\n\n::\n\n'
                # get the script source
                with open(join(root, fname)) as f:
                    for line in f:
                        out += '    ' + line.rstrip() + '\n'
                out += '\n\nOutput\n^^^^^^\n\n::\n\n'
                # launch the test program
                p = Popen(runner + ' ' + join(root, fname), shell=True, stdout=PIPE)
                outdata, err = p.communicate()
                # insert the test output
                for line in outdata.split('\n'):
                    out += '    ' + line.rstrip() + '\n'
                # get the object file
                try:
                    with open(join(root, fname + '.obj')) as f:
                        out += '\n\nObject File\n^^^^^^^^^^^\n\n::\n\n'
                        for line in f:
                            out += '    ' + line.rstrip() + '\n'
                except:
                    pass
                # write the output file
                name = fname.replace('.txt', '.rst')
                with open(join(out_dir, prefix + name), 'w') as f:
                    f.write(out)
                names.append(name)
    # generate the index file
    with open(index_file, 'w') as f:
        for n in names:
            base = camelcase_to_underscore(n.replace('.rst', ''))
            f.write("* `" + n.replace('.rst', '') + " <" + prefix + "_" + base + ".html>`_\n")
    return [prefix + n for n in names]

def build_error_list(in_file, out_file):
    with open(in_file) as i:
        with open(out_file, 'w') as f:
            f.write("\n")
            for line in i:
                code, val = line.split(' ', 1)
                f.write("* **" + code + "** - " + val.rstrip() + "\n")
            f.write("\n")

def doxygen():
    """Run Doxygen."""
    Popen("doxygen", shell=True, stdout=PIPE).communicate()

if __name__ == "__main__":
    print "Warning: Ensure that the Release build is up-to-date!"
    build()
