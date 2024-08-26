#!/bin/sh
set -euf

echo "TODO: Currently only supports macOS ARM64"

# Build client
(cd $CLI_REPO_PATH && cargo build)

# macOS (ARM64)
rm -rf ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_arm64.dylib
cp -r $CLI_REPO_PATH/target/debug/librivet_toolchain_ffi.dylib ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_arm64.dylib
rm -rf ./Assets/Rivet/Editor/Native/cli/rivet-aarch64-apple
cp -r $CLI_REPO_PATH/target/debug/rivet ./Assets/Rivet/Editor/Native/cli/rivet-aarch64-apple
