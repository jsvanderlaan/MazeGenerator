import { Component } from '@angular/core';

@Component({
    selector: 'app-exercise-generator',
    templateUrl: './exercise-generator.component.html',
  })
  export class ExerciseGeneratorComponent {
      result: string;

      getExercise = () =>
      {
        this.result = getRandom(exercises);
      }


  }

  const exercises = [
      "planken",
      "pullup",
      "burpie",
  ]

  function getRandom(arr: string[]) {
      return arr[Math.floor(Math.random() * arr.length)];
  }