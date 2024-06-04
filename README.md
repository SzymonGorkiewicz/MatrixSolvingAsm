# MatrixGaussSolver

The MatrixGaussSolver project represents an advanced implementation that
combines the high-level language C# with low-level optimization using assembly
language. Its primary objective is to effectively solve systems of linear equations
through the application of the Gaussian elimination method. The project also
incorporates technologies that enable optimization of algorithms and operations at
the hardware level.
In C#, the user interface, data management, and the main Gaussian elimination
algorithm are implemented. This algorithm efficiently solves systems of linear
equations by transforming the matrix into row-echelon form. However, for
performance optimization purposes, key code segments have been rewritten in
assembly language, making efficient use of vector instructions.

## Setting up the project

1. Install Visual Studio [VS](https://visualstudio.microsoft.com/pl/vs/)
  * The project is configured for 2022 version of Visual Studio, so i recommend you to download this version.
2. Pull the repository.
3. Open project using **.sln** file
4. Project is prepared to run in ***release*** mode in order to check if my assembly code algorithm is faster than the Visual Studio C# optimizer.
* So i recommend changing it to release mode
     ![release](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/7c7a67bf-6682-41d1-929c-d0a13f9b3c06)
5. You also need to provide path to the ***output data*** and the ***DLL***
* Assembly DLL path in ***Program.cs*** file line **31**
  ![line31programcs](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/190a7912-77a9-417b-94ee-cc1f0e183bb7)
* Assembly output file path ***Program.cs*** file line **71**
  ![OutputLineASM](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/4543b1dc-0a5b-41f3-8fa9-554f42b5d5af)
* C# output file path ***Form1.cs*** file line **102**
  ![C#OutputLine](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/bcb2b779-ab32-4d5a-982c-b59b9970e741)
## Running the project
6. Now you can run the project using release mode
  * You should see the gui
     ![gui](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/3e9d5351-ceb2-49cc-a218-211e6ed2cbef)

     In the slider you can provide number of threads you want do calculations on from ***1 to 64***

  * Load data button
    ![loadingData](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/9cee52d3-3e55-4399-a613-eabf445ad262)
    * Now you can select .txt file with matrices.

  * Input data format
    
    ![input data](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/027afcfd-ab44-483f-b73f-d93b281804a0)
    * One matrix example of input data in the .txt file.
  * When you choose correctly formatted data you should see that the file got read.
    ![loaded file](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/6f6b691c-f832-4461-9781-14e467c88e7b)

7. Calculations
* Now you can choose rather you want to perform calculations based on C# algorithm or assembly algorithm.
     * C# calculations using 16 threads took about 7miliseconds.
     ![16ThreadsC#](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/086c2626-8934-49ec-acbe-ab46b5d42ff7)

     * Assembly calculations are much faster and took only 2.5miliseconds.
     ![16threadsASM](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/8934e782-2b22-4550-b19f-ce7c5f37f031)
   
8. Now you can see outputs of the calculations in the ***output_data*** folder
    * C# output. <br /><br />
    ![C#Output](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/462aa96d-fcff-47f6-9e91-1695d215a608)

    * Assembly output. <br /><br />
    ![ASMOutput](https://github.com/SzymonGorkiewicz/MatrixSolvingAsm/assets/92310752/cd89c220-9142-49e3-8457-75e46e33c0f7)

## Contributing

Pull requests are welcome.
