#!/bin/sh
set -euf

# Linux (x86_64)
rm -rf ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_linux_x86_64.so
cp -r $CLI_REPO_PATH/target/x86_64-unknown-linux-gnu/release/librivet_toolchain_ffi.so ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_linux_x86_64.so
rm -rf ./Assets/Rivet/Editor/Native/cli/rivet-linux  
cp -r $CLI_REPO_PATH/target/x86_64-unknown-linux-gnu/release/rivet ./Assets/Rivet/Editor/Native/cli/rivet-linux

# Windows (x86_64)
rm -rf ./Assets/Rivet/Editor/Native/rivet_toolchain_ffi_windows_x86_64.dll
cp -r $CLI_REPO_PATH/target/x86_64-pc-windows-gnu/release/rivet_toolchain_ffi.dll ./Assets/Rivet/Editor/Native/rivet_toolchain_ffi_windows_x86_64.dll
rm -rf ./Assets/Rivet/Editor/Native/cli/rivet-windows.exe
cp -r $CLI_REPO_PATH/target/x86_64-pc-windows-gnu/release/rivet.exe ./Assets/Rivet/Editor/Native/cli/rivet-windows.exe

# macOS (x86_64)
rm -rf ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_x86_64.dylib
cp -r $CLI_REPO_PATH/target/x86_64-apple-darwin/release/librivet_toolchain_ffi.dylib ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_x86_64.dylib
rm -rf ./Assets/Rivet/Editor/Native/cli/rivet-x86-apple
cp -r $CLI_REPO_PATH/target/x86_64-apple-darwin/release/rivet ./Assets/Rivet/Editor/Native/cli/rivet-x86-apple

# macOS (ARM64)
rm -rf ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_arm64.dylib
cp -r $CLI_REPO_PATH/target/aarch64-apple-darwin/release/librivet_toolchain_ffi.dylib ./Assets/Rivet/Editor/Native/librivet_toolchain_ffi_macos_arm64.dylib
rm -rf ./Assets/Rivet/Editor/Native/cli/rivet-aarch64-apple
cp -r $CLI_REPO_PATH/target/aarch64-apple-darwin/release/rivet ./Assets/Rivet/Editor/Native/cli/rivet-aarch64-apple