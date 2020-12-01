# Stubborn

A simple YAML serializer for stubborn people who insist that machine-generated
output should look just as if it was formatted by hand.

## Why?

Because YAML is supposed to be primarily a human-readable data format, yet
common serialization is focused simply on storing data. There might be options
for some formatting choices, like property naming or level of indentation, but
that's it.

On the other hand, YAML files created and maintained by humans feature much
more elements like vertical whitespace or comments of many different styles at
various places.

_Stubborn_ aims to allow such high level of control over formatting even with
machine-generated YAML files, while keeping out of the way and just doing the
right thing when possible.

## Goals

- Keep It Simple & Stupid
- Generate valid YAML 1.2 documents
- Serialize all common data types
- Perform automatic recursive serialization through .NET reflection
- Allow high level of output formatting customization

## Non-goals

- Deserialization
- Support for every YAML feature (e.g. anchors or non-scalar keys)
- Capability to generate every YAML style possible
- Performance

## Examples

See [example-default.yaml](example-default.yaml) and compare it with
[example-stubborn.yaml](example-stubborn.yaml). Look at
[Stubborn.Tests/Example.cs](Stubborn.Tests/Example.cs) to learn how
the effects were achieved.

- The file contains comments of various styles at various levels
- There are blank lines between different sections
- Properties are ordered logically even across inheritance chain
  (the "name" of a step always at the top etc.)
- Properties with default values omitted
- Explanatory comment next to a magic number (see "timeoutSec")
- Consistent formatting of strings (script blocks, quotes)
- Consistent formatting of lists (see the "checks" property)
- ...

## Licence

MIT

## Contributing

Bug fixes and improvements are welcome. Add tests for updated behaviors.
