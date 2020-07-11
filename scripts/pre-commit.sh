#!/bin/bash

echo "Running pre-commit hook"
./scripts/run-tests.sh

if [ $? -ne 0 ]; then
    echo "Tests are failing, cannot commit"
    exit 1
fi
