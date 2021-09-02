# Range Selector

A simple range selection control for Windows Forms.
It allows the user to select a range (min, max) from a predefined interval.
The selected interval is drawn as a rectangle of a predefined color either over the background of the control or over an image.

## Properties

Several properties of the control are exposed:

| Name      | Description |
| ----------- | ----------- |
| Min | Minimum value of the range selector |
| Max | Maximum value of the range selector |
| SelectedMin | Minimum value of the selected range |
| SelectedMax | Maximum value of the selected range |
| SelectionColor | Color of the selected region |
| Step | Step between values |
| BackgroundColor | Background color (only used when no image was selected) |
| Image | Background image |
| Label | Description for the range selector |

The control also offers an event **SelectionChanged** fired every time the selected range changes.

## Examples

Some examples

Simple color-based range selector.

<img src="/screenshots/example_0.PNG">

Control allowing the user to select a spectral range.

<img src="/screenshots/example_1.PNG">

Selection of a mass range on a mass spectrum.

<img src="/screenshots/example_3.PNG">
