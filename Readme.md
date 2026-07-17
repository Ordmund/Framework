# Game Framework

A set of reusable tools and base systems that can be used in any game project,
built while developing my own game and moved here whenever something was
general enough to be useful elsewhere.

## Core systems

- **MVC game object framework** — Model/View/Controller base classes for game
  objects, wired through Zenject for dependency injection. Includes pooling,
  addressable-based views, and lifetime tracking for controllers.

- **ScriptableObject definitions & behaviours** — A data-driven layer for
  defining game entities (items, world objects, creatures, etc.) as
  ScriptableObject assets, with associated behaviour classes and registries
  for lookup. Includes custom editor tooling for creating and managing these
  assets in the Unity Editor.

- **Supporting utilities** — Resource loading, file I/O, async task helpers,
  and a handful of reusable UI components (sliders, toggles, buttons, cyclic
  option selectors).

## Status

Actively evolving alongside my current game project. This is my personal
toolkit made public as a code sample — not a maintained/supported library.
Use at your own risk, no guarantees on stability or backward compatibility.