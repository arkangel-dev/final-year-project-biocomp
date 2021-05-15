from NeuralNetworks import Brain
import random
import sys
import time
import numpy as np

class Population:
    Candidates = []
    TrainingSet = []

    CurrentGenerationTotalScore = 0
    PopulationCount = 5

    def __init__(self):


        data_raw = ""
        with open("data3.txt", "r") as d: data_raw = d.read().split("\n")
        processed = []
        for s in data_raw:
            up = s.split()
            processed.append([ [float(x) for x in up[:]], int(up[6])])

        self.TrainingSet = processed[:100]

        self.Candidates = []
        for i in range(self.PopulationCount):
            self.Candidates.append(Candidate())
            print(id(self.Candidates[i].Brain))
        print("======")
            
        for candidate in self.Candidates:
            print(id(candidate.Brain.GetBrainConfig()[0]))

    def RunExamAndSort(self):
        print("Running examination...")
        for candidate in self.Candidates:
            
            candidate.RunTest(self.TrainingSet)
            
        self.Candidates.sort()

    def RouletteSelection(self):
        print("Selecting parents...")
        MatingWeights = []
        for candidate in self.Candidates:
            MatingWeights.append(candidate.Score / self.CurrentGenerationTotalScore)
        
        for _ in range(self.PopulationCount):
            parents = random.choices(self.Candidates, weights=MatingWeights, k=2)


    def RunSimulation(self, interation = 1):
        self.RunExamAndSort()


class Candidate:
    Brain = None
    Score = 0

    def __init__(self):
        
        self.Brain = Brain(
            hiddenLayerCount = 2,
            nodesPerHiddenLayer = 5,
            outputLayerCount = 2,
            inputLayerCount = 7
        )

    def RunTest(self, testset):
        self.Score = 0
        for test in testset:
            _input = test[0]
            expectedOutput = test[1]
            processedData = self.Brain.ProcessData(_input)
            actualOutput = 0 if processedData[0] > processedData[1] else 1

            if (actualOutput == expectedOutput):
                self.Score += 1
        self.Score = self.Score / len(testset)
        return self.Score

    def GetWeightArray(self):
        return self.Brain.GetBrainConfiguration()

    def SetWeightArray(self, config):
        return self.Brain.SetBrainConfiguration(config)

    def __lt__(self, other):
        return self.Score < other
    
    def __gt__(self, other):
        return self.Score > other

    def __eq__(self, other):
        return self.Score == other.Score

    def __repr__(self):
        return "<" + type(self).__name__ + "." +str(id(self)) + "-Score:" + str(self.Score) + ">"