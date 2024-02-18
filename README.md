# WhyIUseAwait
A simple demo of how to use await with Winforms and .NET.

# How to install
* Clone/Download this repository.
* Load the solution in Visual Studio.

The project should be ready to run now.

# How to use
This application demo offers two buttons.

The first button runs the "long calculation" using a standard method.

The second button calls the exact same method, but wrapped in a Task.Run().

Run the application, press either button, and try to drag the window.

See the different results?

The left button blocks the UI when running the calculation.

The right button does not block the UI (by running the calculation in an awaited Task).

Examine the code, and you'll see how it works.
