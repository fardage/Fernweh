#!/usr/bin/env bash

sed -i -e "s/HEREMAPS_API_KEY/$HEREMAPS_API_KEY/g" Fernweh/Services/Credentials.cs
sed -i -e "s/APPCENTER_API_KEY/$APPCENTER_API_KEY/g" Fernweh/Services/Credentials.cs
