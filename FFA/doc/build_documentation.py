#!/usr/bin/env python
# Author: Jacob Peddicord <peddicord.17@osu.edu>

import os
import re
from os.path import join, basename
from subprocess import call, Popen, PIPE
from shutil import rmtree, copytree, copyfile
from docutils.core import publish_file
from docutils.writers import manpage, html4css1

global_cwd = os.getcwd()

def preserve_cwd(function):
   def decorator(*args, **kwargs):
      cwd = os.getcwd()
      try:
          return function(*args, **kwargs)
      finally:
          os.chdir(cwd)
   return decorator

### Builders ###

def build():
    """The main builder."""
    try:
        rmtree('html')
    except: pass
    os.mkdir('html')
    
    print "*** Building Assembler documentation"
    build_assembler()
    copytree("../Assembler/doc/out/html/", "html/assembler/")
    print "*** Building Linker documentation"
    build_linker()
    copytree("../Linker/doc/out/html/", "html/linker/")
    print "*** Building Simulator documentation"
    build_simulator()
    copytree("../Simulator/doc/out/html/", "html/simulator/")

    print "*** Building toplevel"
    build_rst_dir('src', 'html', 'rst-top')
    copyfile('template/style.css', 'html/style.css')

@preserve_cwd
def build_assembler():
    """Documentation builder for Assembler"""
    
    os.chdir("../Assembler/doc")
    
    # clean up old builds
    try:
        rmtree('tmp')
        rmtree('out')
    except: pass
    os.makedirs('tmp/tests')

    # build
    print "Building DED"
    build_ded('../', 'tmp/ded.rst')
    convert_rst('tmp/ded.rst', 'tmp/ded.html', 'rst-assembler')
    create_dox_wrapper('tmp/ded.rst', 'tmp/ded.dox')
    print "Running test scripts"
    scripts = build_test_scripts('../Tests/Programs', '../bin/Release/Assembler.exe', 'tmp/tests', 'testfile_', 'tmp/testscript_index.rst')
    for script in scripts:
        convert_rst(join('tmp/tests', script), join('tmp', script.replace('.rst', '.html')), 'rst-assembler')
        create_dox_wrapper(join('tmp/tests', script), join('tmp', script.replace('.rst', '.dox')))
    print "Creating error listing"
    build_error_list('../Resources/errors.txt', 'tmp/errorlist.rst')
    print "Building manual"
    build_rst_dir('src', 'tmp', 'rst-assembler')
    print "Running Doxygen"
    doxygen()
    print "Copying images"
    copytree("src/images", "out/html/images")

@preserve_cwd
def build_linker():
    """Documentation builder for Linker"""
    
    os.chdir("../Linker/doc")
    
    # clean up old builds
    try:
        rmtree('tmp')
        rmtree('out')
    except: pass
    os.makedirs('tmp/tests')
    
    # build
    print "Building DED"
    build_ded('../', 'tmp/ded.rst')
    convert_rst('tmp/ded.rst', 'tmp/ded.html', 'rst-linker')
    create_dox_wrapper('tmp/ded.rst', 'tmp/ded.dox')
    print "Running test scripts"
    scripts = build_linker_tests('../Tests/Programs', '../bin/Release/Linker.exe', 'tmp/tests', 'testlink_', 'tmp/testlink_index.rst')
    for script in scripts:
        convert_rst(join('tmp/tests', script), join('tmp', script.replace('.rst', '.html')), 'rst-linker')
        create_dox_wrapper(join('tmp/tests', script), join('tmp', script.replace('.rst', '.dox')))
    print "Creating error listing"
    build_error_list('../Resources/Errors.txt', 'tmp/errorlist.rst')
    print "Building manual"
    build_rst_dir('src', 'tmp', 'rst-linker')
    print "Running Doxygen"
    doxygen()
    print "Copying images"
    copytree("src/images", "out/html/images")
    
@preserve_cwd
def build_simulator():
    """Documentation builder for Simulator"""
    
    os.chdir("../Simulator/doc")
    
    # clean up old builds
    try:
        rmtree('tmp')
        rmtree('out')
    except: pass
    os.makedirs('tmp/tests')
    
    # build
    print "Building DED"
    build_ded('../', 'tmp/ded.rst')
    convert_rst('tmp/ded.rst', 'tmp/ded.html', 'rst-simulator')
    create_dox_wrapper('tmp/ded.rst', 'tmp/ded.dox')
    print "Running test scripts"
    scripts = build_sim_tests('../Tests/Programs', '../bin/Release/Simulator.exe', 'tmp/tests', 'testsim_', 'tmp/testsim_index.rst', '.ffa')
    for script in scripts:
        convert_rst(join('tmp/tests', script), join('tmp', script.replace('.rst', '.html')), 'rst-simulator')
        create_dox_wrapper(join('tmp/tests', script), join('tmp', script.replace('.rst', '.dox')))
    print "Creating error listing"
    build_error_list('../Resources/errors.txt', 'tmp/errorlist.rst')
    print "Building manual"
    build_rst_dir('src', 'tmp', 'rst-simulator')
    print "Running Doxygen"
    doxygen()
    print "Copying images"
    copytree("src/images", "out/html/images")

### Utility functions ###

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

def build_rst_dir(directory, out_dir, template):
    """Generate HTML for all RST files in the given directory. Additionally, create dox wrappers."""
    for root, dirs, files in os.walk(directory):
        for fname in files:
            if fname.endswith('.rst'):
                convert_rst(join(root, fname), join(out_dir, fname.replace('.rst', '.html')), template)
                create_dox_wrapper(join(root, fname), join(out_dir, fname.replace('.rst', '.dox')))

def convert_rst(rst_file, out_file, template):
    """Use ReST to convert the given file into HTML."""
    publish_file(source_path=rst_file, destination_path=out_file, writer=html4css1.Writer(), settings_overrides={'template': join(global_cwd, 'template/' + template + '.html')})

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

def build_test_scripts(directory, runner, out_dir, prefix, index_file, ext='.txt'):
    """Copy the test script input, run the script, and copy output to an RST source."""
    names = []
    for root, dirs, files in os.walk(directory):
        files.sort()
        for fname in files:
            if fname.endswith(ext):
                print "    " + fname
                out = fname + '\n' + '`'*len(fname) + '\n\n.. contents::'
                # get the info file
                try:
                    with open(join(root, fname.replace(ext, '.info'))) as f:
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
                name = fname.replace(ext, '.rst')
                with open(join(out_dir, prefix + name), 'w') as f:
                    f.write(out)
                names.append(name)
        break
    # generate the index file
    with open(index_file, 'w') as f:
        for n in names:
            base = camelcase_to_underscore(n.replace('.rst', ''))
            f.write("* `" + n.replace('.rst', '') + " <" + prefix + "_" + base + ".html>`_\n")
    return [prefix + n for n in names]

def build_linker_tests(directory, runner, out_dir, prefix, index_file, ext='.objtxt'):
    """Run all linker tests and store output."""
    names = []
    for root, dirs, files in os.walk(directory):
        dirs.sort()
        # tests are grouped into directories
        for dname in dirs:
            print "    " + dname
            out = dname + '\n' + '`'*len(dname) + '\n\n.. contents::'
            # get the info file
            try:
                with open(join(root, dname, 'info')) as f:
                    out += '\n\n' + f.read()
            except:
                pass
            out += '\n\nInput\n^^^^^'
            # script sources
            sources = []
            for oroot, odirs, ofiles in os.walk(join(root, dname, 'objects')):
                ofiles.sort()
                for fname in ofiles:
                    if not fname.endswith(ext):
                        continue
                    out += '\n\n' + fname + '\n' + '~'*len(fname) + '\n\n::\n\n'
                    try:
                        with open(join(oroot, fname)) as f:
                            for line in f:
                                out += '    ' + line.rstrip() + '\n'
                    except:
                        pass
                    sources.append(fname)
                break
            out += '\n\nOutput\n^^^^^^\n\n::\n\n'
            # launch the linker
            p = Popen([join(os.getcwd(), runner)] + sources, stdout=PIPE, cwd=oroot)
            outdata, err = p.communicate()
            # grab and insert the testing output
            for line in outdata.split('\n'):
                out += '    ' + line.rstrip() + '\n'
            # grab the load file
            try:
                with open(join(oroot, sources[0].replace(ext, '.ffa'))) as f:
                    out += '\n\nLoad File\n^^^^^^^^^\n\n::\n\n'
                    for line in f:
                        out += '    ' + line.rstrip() + '\n'
            except:
                pass
            # write the output
            name = dname + '.rst'
            with open(join(out_dir, prefix + name), 'w') as f:
                f.write(out)
            names.append(name)
        break
    # generate the index
    with open(index_file, 'w') as f:
        for n in names:
            base = camelcase_to_underscore(n.replace('.rst', ''))
            f.write("* `" + n.replace('.rst', '') + " <" + prefix + "_" + base + ".html>`_\n")
    return [prefix + n for n in names]

def build_sim_tests(directory, runner, out_dir, prefix, index_file, ext='.ffa'):
    """Copy the test script input, run the script, and copy output to an RST source."""
    names = []
    for root, dirs, files in os.walk(directory):
        files.sort()
        for fname in files:
            if fname.endswith(ext):
                print "    " + fname
                out = fname + '\n' + '`'*len(fname) + '\n\n.. contents::'
                # get the info file
                try:
                    with open(join(root, fname.replace(ext, '.info'))) as f:
                        out += '\n\n' + f.read()
                except:
                    pass
                out += '\n\nInput\n^^^^^\n\n::\n\n'
                # get the script source
                with open(join(root, fname)) as f:
                    for line in f:
                        out += '    ' + line.rstrip() + '\n'
                # check for test script input
                try:
                    with open(join(root, fname.replace('.ffa', '.input'))) as f:
                        scriptin = f.read()
                        print "        (input file found)"
                except:
                    scriptin = ''
                out += '\n\nOutput\n^^^^^^\n\n::\n\n'
                # launch the test program
                p = Popen(runner + ' ' + join(root, fname), shell=True, stdout=PIPE, stdin=PIPE)
                outdata, err = p.communicate(input=scriptin)
                # insert the test output
                for line in outdata.split('\n'):
                    out += '    ' + line.rstrip() + '\n'
                out += '\n\nDebug Mode\n~~~~~~~~~~\n\n::\n\n'
                # launch the test program again, with debug
                p = Popen(runner + ' -d ' + join(root, fname), shell=True, stdout=PIPE, stdin=PIPE)
                outdata, err = p.communicate(input=scriptin)
                # insert the debug output
                for line in outdata.split('\n'):
                    out += '    ' + line.rstrip() + '\n'
                # write the output file
                name = fname.replace(ext, '.rst')
                with open(join(out_dir, prefix + name), 'w') as f:
                    f.write(out)
                names.append(name)
        break
    # generate the index file
    with open(index_file, 'w') as f:
        for n in names:
            base = camelcase_to_underscore(n.replace('.rst', ''))
            f.write("* `" + n.replace('.rst', '') + " <" + prefix + "_" + base + ".html>`_\n")
    return [prefix + n for n in names]

def build_error_list(in_file, out_file):
    """Build an error listing."""
    with open(in_file) as i:
        with open(out_file, 'w') as f:
            f.write("\n")
            for line in i:
                if len(line.strip()) > 0:
                    code, val = line.split(' ', 1)
                    f.write("* **" + code + "** - " + val.rstrip() + "\n")
            f.write("\n")

def doxygen():
    """Run Doxygen."""
    os.mkdir("out")
    Popen("doxygen", shell=True, stdout=PIPE).communicate()

if __name__ == "__main__":
    print "Warning: Ensure that the Release build is up-to-date!"
    build()
