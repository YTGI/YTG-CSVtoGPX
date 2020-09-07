# YTG-CSVtoGPX-UI
## Yasgar Technology Group, Inc.
### Convert CSV file to GPX file for Waypoints

<p>This is a simple .NET Framework (4.7.2) WPF application that imports a CSV file in the format located in the \Files folder.</p>
<p>It generates a GPX file that is compliant with GARMIN products. It is likely compliant with most GPS products, but I don't own any others to test with.</p>
<p>If you want a simple way to import some data you have into a file that will display in your GPS for navigation, then this is a simple way to accomplish that.</p>
<p><b>Please Note:</b></p>
<p>We're releasing this utility as an open source project, however, we don't have any intention on improving or maintaining it past what we need for our own uses.</p>
<p><b>Also Note:</b></p>
<p>All of your entries MUST have a Latitude and Longitude in decimal format present. The CSV parser blows chunks on empty fields! I didn't look into whether this could be overridden, as I really don't want them in my GPX file anyway.</p>

## Testing
<p>GPX file tested on a Garmin Oregon 650t</p>
<p>GPX file tested with Google Maps (https://www.google.com/maps/d/?hl=en) (Ignores symbol)</p>
<p>GPX file tested with Goolge Earth Pro (Uses flag symbol)</p>

## Dependencies
<p>
<ul>
<li>.NET Framework 4.7.2</li>
<li>CsvHelper version 15.0.5</li>
</ul>
</p>

<p>2020 - <a href="http://www.ytgi.com" target="_blank">Yasgar Technology Group, Inc.</a></p>
