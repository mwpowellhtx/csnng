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

set (NNG_GIT_URL git@github.com:nanomsg/nng.git)
set (NNG_GIT_TAG HEAD)
set (NNG_GIT_REPO_DIR ${CMAKE_CURRENT_SOURCE_DIR}/repos/nng)

if ("${CSNNG_ENABLE_ZEROTIER}" STREQUAL "")
    set (CSNNG_ENABLE_ZEROTIER OFF)
endif ()

ExternalProject_Add (nng
    GIT_REPOSITORY ${NNG_GIT_URL}
    GIT_TAG ${NNG_GIT_TAG}
    GIT_PROGRESS ON
    SOURCE_DIR ${NNG_GIT_REPO_DIR}
    CMAKE_ARGS -DCMAKE_INSTALL_PREFIX:STRING=${NNG_INSTALL_PREFIX} -DNNG_ENABLE_COVERAGE:BOOL=OFF -DNNG_ENABLE_DOC:BOOL=OFF -DNNG_ENABLE_NNGCAT:BOOL=OFF -DNNG_ENABLE_TESTS:BOOL=OFF -DNNG_ENABLE_TOOLS:BOOL=OFF -DNNG_ENABLE_ZEROTIER:BOOL=${CSNNG_ENABLE_ZEROTIER}
    LOG_DOWNLOAD ON
)

message (STATUS "NNG '${NNG_GIT_TAG}' build configured.")

set (NNG_INSTALL_PREFIX ${CMAKE_CURRENT_SOURCE_DIR}/nngbuild)
set (NNG_GIT_REPO_SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/repos/nng/src)
