# CURGLAFF Dialogue Box
A generic system for displaying text in Unity, designed to allow extending to support any text output imaginable.

The intended use case for this package is RPG-style dialogue boxes and other non-instant displays (e.g. scrolling billboards). The package currently includes an outputter for
Unity's official text component package, TextMesh Pro, and an outputter that displays characters one by one ("typewriter effect," I've seen this called, and it's fitting).
The intention is that multiple outputters should be possible to chain together in order to create whatever effect you want the text to have, but at present it's not quite there.

The package includes systems for parsing **macros** and **custom escape codes** within passed strings. Macros are character sequences which the engine will replace with other 
characters at runtime (e.g. "%plyr1" => "Edgar Wright", "%hometown" => "Jundrygrance", "%loudbrk" => "\n\b", etc.); escape codes produce some arbitrary effect when they are evaluated (e.g. changing the text color, inserting an icon, cuing a character animation outside the box, etc.).
