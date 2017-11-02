#
#   Copyright (c) 2012 Martin Sustrik  All rights reserved.
#   Copyright (c) 2013 GoPivotal, Inc.  All rights reserved.
#   Copyright (c) 2015-2016 Jack R. Dunaway. All rights reserved.
#   Copyright 2016 Franklin "Snaipe" Mathieu <franklinmathieu@gmail.com>
#   Copyright 2017 Garrett D'Amore <garrett@damore.org>
#   Copyright (c) 2017 Michael W. Powell <mwpowellhtx@gmail.com> All rights reserved.
#
#   Permission is hereby granted, free of charge, to any person obtaining a copy
#   of this software and associated documentation files (the "Software"),
#   to deal in the Software without restriction, including without limitation
#   the rights to use, copy, modify, merge, publish, distribute, sublicense,
#   and/or sell copies of the Software, and to permit persons to whom
#   the Software is furnished to do so, subject to the following conditions:
#
#   The above copyright notice and this permission notice shall be included
#   in all copies or substantial portions of the Software.
#
#   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
#   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
#   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
#   FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
#   IN THE SOFTWARE.
#
cmake_minimum_required (VERSION 3.1)

# Analog to the Visual Studio Solution paradigm.
project (CsNng CXX)

include (ExternalProject)
# TODO: TBD: Processor Count was included for purposes of supporting a Boost build, but since then I've gone a different direction
include (ProcessorCount)
include (GNUInstallDirs)

option (CMAKE_CONFIGURATION_TYPES "" Debug)
option (CMAKE_INSTALL_PREFIX "" install)

option (CSNNG_ENABLE_DOC "Enable building documentation." ON)
option (CSNNG_ENABLE_TESTS "Build and run tests" ON)
option (CSNNG_ENABLE_XTRA_TOOLS "Build extra tools" OFF)
option (CSNNG_ENABLE_NNGCAT "Enable building nngcat utility." ${NNG_TOOLS})
option (CSNNG_ENABLE_COVERAGE "Enable coverage reporting." OFF)
option (CSNNG_ENABLE_ZEROTIER "Enable ZeroTier protocol." OFF)

find_package (Threads REQUIRED)

macro (verify_git_exists)

    find_package (Git REQUIRED)

    if (GIT_FOUND)
        message (STATUS "Git found: ${GIT_EXECUTABLE} ${GIT_VERSION_STRING}")
    else ()
        message (FATAL_ERROR "Git required: not found.")
    endif()

endmacro ()

verify_git_exists ()

# Should be running this from the top-most repository directory. This
# should also go without saying, but may not be as obvious to all.

set (CSNNG_SWIG_DIR ${CsNng_SOURCE_DIR}/swig)

include (nanomsg2.cmake)

# # TODO: TBD: it may be challenging to mix C# Visual Studio Project/Solution paradigms with that of the CMake directed build
# add_subdirectory (src)
