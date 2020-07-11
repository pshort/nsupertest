#!/bin/bash
cd "${0%/*}/.."

echo "Running pre-commit tests"
dotnet test

