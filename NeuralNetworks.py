from io import BufferedIOBase
from types import LambdaType
import random as rand
import numpy as np

class Brain:
    HiddenLayerCount = 0
    NodesPerHiddenLayer = 0
    OutputLayerCount = 0
    InputLayerCount = 0
    seed = 0

    Layers = []

    def __init__(self, hiddenLayerCount, nodesPerHiddenLayer, outputLayerCount, inputLayerCount) -> None:
        self.HiddenLayerCount = hiddenLayerCount
        self.NodesPerHiddenLayer = nodesPerHiddenLayer
        self.OutputLayerCount = outputLayerCount
        self.InputLayerCount = inputLayerCount



        # Generate the first hidden layer. This layers neurons will have a
        # limited amount of inputs. Aka limited number of input weights
        self.GenerateNewLayer(self.NodesPerHiddenLayer, self.InputLayerCount)

        # Generate the other hidden layers. The neurons in this layer will have
        # a the same number of inputs as the number of neurons in the previous layer
        for _ in range(self.HiddenLayerCount - 1):
            self.GenerateNewLayer(self.NodesPerHiddenLayer, self.NodesPerHiddenLayer)
        
        # Generate the final output layer. This layer will have the number of neurons specified
        # in the outputLayerCount parameter
        self.GenerateNewLayer(self.OutputLayerCount, self.NodesPerHiddenLayer)

    def ProcessData(self, Input):
        # Pass in the data into the first layer
        for neuron in self.Layers[0]:
            neuron.CalculateOutput(Input)

        # Process the data for the other hidden layers
        for i in range(1, len(self.Layers) - 1):
            for neuron in self.Layers[i]:
                neuron.CalculateOutput(self.GetValuesFromLayer(self.Layers[i - 1]))
        
        # Process data for the output layer
        for neuron in self.Layers[-1]:
            neuron.CalculateOutput(self.GetValuesFromLayer(self.Layers[-2]))

        # return the output from the brain
        return self.GetValuesFromLayer(self.Layers[-1])
        

    def GetValuesFromLayer(self, layer):
        values = []
        for neuron in layer:
            values.append(neuron.Output)
        return values

    def GetBrainConfiguration(self):
        # This function will return a list of weight and biases
        # so they can be manipulated by the genetic algorithm
        weights = []
        for layer in self.Layers:
            for neuron in layer:
                weights.extend(neuron.Weights)
        return weights
    
    def SetBrainConfiguration(self, weights):
        # This function will apply the configuration that was manipulated by the
        # genetic algorithm
        offset = 0
        for layer in self.Layers:
            for neuron in layer:
                count = len(neuron.Weights)
                neuron.Weights = weights[:count]
                weights = weights[count:]

    def GenerateNewLayer(self, NodeCount, InputCount):
        NewLayer = []
        for _ in range(NodeCount):
            NewLayer.append(Neuron(InputCount))
        self.Layers.append(NewLayer)


class Neuron:
    Weights = []
    Bias = 0.0
    Output = 0

    def __init__(self, InputCount) -> None:
 
        self.Weights = np.random.uniform(1, -1, InputCount)
   
  

    def CalculateOutput(self, Input):
        self.Output = 0.0
        for i in range(len(Input)):
            self.Output += Input[i] * self.Weights[i]
        self.Output += self.Bias
        return self.Output