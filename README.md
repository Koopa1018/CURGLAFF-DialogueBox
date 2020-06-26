# CURGLAFF Dialogue Box
A generic system for displaying text in Unity, designed to allow extending to support any text output imaginable.

The intended use case for this package is RPG-style dialogue boxes and other non-instant displays (e.g. scrolling billboards). The package currently includes an outputter for
Unity's official text displayer package, TextMesh Pro, and an outputter that displays characters one by one ("typewriter effect," I've seen this called, and it's fitting).
The intention is that multiple outputters should be possible to chain together in order to create whatever effect you want the text to have, but at present it's not quite there.
