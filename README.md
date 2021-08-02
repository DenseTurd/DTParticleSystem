# DTParticleSystem
Surplus to requirement particle system, hopefully a nice demonstration of tooling ability though :)

My initial goal was to create a GameObject particle system with reduced functionality for my designers to use as they didn't like the large Unity particle system inspector and I wanted the convinience of having GameObjects as particles.

I tried using compute shaders to calculate the transforms of the gameobjects, but getting data buffers back from the GPU was too slow to do every frame, so I reverted to doing it on the CPU and decided it would be a nice portfolio piece even with it's limited capabilities. 

The custom editor is designed to have only the functionality my designers need for this particular project, hopefully it's not as scary for them to use.

I also made the default settings fire-esque, demonstrating much of the functionality.

Getting the preview to work was hard! So I've moved on and made it play mode only.
