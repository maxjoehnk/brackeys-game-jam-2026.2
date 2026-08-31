#!/usr/bin/env bash
set -eo pipefail

echo "Create keychain profile"
xcrun notarytool store-credentials "notarytool-profile" --apple-id "$MACOS_NOTARIZATION_APPLE_ID" --team-id "$MACOS_NOTARIZATION_TEAM_ID" --password "$MACOS_NOTARIZATION_PASSWORD"

# We can't notarize an app bundle directly, but we need to compress it as an archive.
# Therefore, we create a zip file containing our app bundle, so that we can send it to the
# notarization service

echo "Creating temp notarization archive"
ditto -c -k --keepParent "build/Hell House.app" "notarization.zip"

# Here we send the notarization request to the Apple's Notarization service, waiting for the result.
# This typically takes a few seconds inside a CI environment, but it might take more depending on the App
# characteristics. Visit the Notarization docs for more information and strategies on how to optimize it if
# you're curious

echo "Notarize app"
submission_json="$(xcrun notarytool submit "notarization.zip" --keychain-profile "notarytool-profile" --wait --output-format json)"
echo "$submission_json"

submission_id="$(echo "$submission_json" | jq -r ".id")"
status="$(echo "$submission_json" | jq -r ".status")"

if [ -n "$submission_id" ]; then
  echo "Notarization log for $submission_id"
  xcrun notarytool log "$submission_id" --keychain-profile "notarytool-profile" || true
fi

if [ "$status" != "Accepted" ]; then
  echo "Notarization failed with status: $status" >&2
  exit 1
fi

# Finally, we need to "attach the staple" to our executable, which will allow our app to be
# validated by macOS even when an internet connection is not available.
echo "Attach staple"
xcrun stapler staple "build/Hell House.app"
