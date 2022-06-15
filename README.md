# MeshVis3D

Mesh Vis 3D is intended as a simple viewer for 3d models, allowing for translation, rotation, and scaling of the model.

The visualizer will let you showcase and manipulate stunning 3D models. Additionally, this visualizer will let you experiment with different lighting conditions and post processing effects in realtime.

## Using the visualizer

MeshVis3D waas built using Unity 2021.3.4f1

### Using the dropdown elements
The visualizer starts empty, so you will need to select something to view. There are four dropdown elements along the bottom of the visualizer, which are intended to be used in sequence, from left to right (Prefab, Model, Material, Texture). All things in the visualizer start with selecting a prefab via the first dropdown. On selecting a new prefab, the object is displayed, and a list of component models is populated into the next dropdown box. The final two dropdown elements contain a collection of materials and textures. Selecting a value in these final two collections will apply the selection to the model being viewed.

### Using the transform controls
There are transform controls in the top left of the screen. They consist of a radio group for manipulation mode (translate, rotate, scale), and a button to reset the transform. Transform controls are applied by first selecting a mode, then performing a click-and-drag operation on the model being visualized.
Clicking the reset button sets the model's transform to default.

## Approach
### Forcing process
I wanted to force the user into a certain workflow when interacting with the visualizer. This was intended in part to cut down on chaos and scope. I did not want to spend a lot of time thinking about how to handle if the user selected a texture, for example, before selecting something to display the texture on, for example.
This version of the app (compared to future versions that'd be much more complete) does admittedly little to protect the user form themselves with regards to following the workflow. Nothing prevents them from selecting a model without first selecting a prefab (as evidenced by the default entry in the model list), but the intention is that safeguards would be put in place eventually.

### It's all ephemeral
This project was described as a viewer, so I chose to omit any ability to 'apply' the changes the user may have tampered with.

## Challenges
### New tech, new skills
I've not worked with the xml/css paradigm (generally) or UI Toolkit (specifically) before. I chose to go ahead with UIToolkit anyway (even though it means my UI is rudimentary) because I value learning new things, and this felt like a good place to exercise a bit of that.
