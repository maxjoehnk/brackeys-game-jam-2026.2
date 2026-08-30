#!/usr/bin/env bash
set -e

echo "Packaging as .dmg..."
create-dmg --volname "Hell House" \
	--volicon "build/Hell House.app/Contents/Resources/icon.icns" \
	--window-pos 200 120 \
	--window-size 800 400 \
	--icon-size 100 \
	--icon "Hell House.app" 200 190 \
	--hide-extension "Hell House.app" \
	--app-drop-link 600 185 \
	--codesign "$MACOS_CERTIFICATE_NAME" \
	Loopscape.dmg \
	build
