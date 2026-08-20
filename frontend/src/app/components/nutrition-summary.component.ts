import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NutritionMetricComponent } from './nutrition-metric.component';

@Component({
  selector: 'app-nutrition-summary',
  standalone: true,
  imports: [CommonModule, NutritionMetricComponent],
  template: `
    <div class="bg-[#121215] border border-zinc-800/80 rounded-xl divide-y md:divide-y-0 md:divide-x divide-zinc-800/60 grid grid-cols-2 md:grid-cols-5 shadow-inner">
      <app-nutrition-metric
        label="Energy"
        [value]="energy()"
        unit="kcal"
        colorClass="text-[#ea580c]"
      ></app-nutrition-metric>
      <app-nutrition-metric
        label="Protein"
        [value]="protein()"
        unit="g"
        colorClass="text-sky-400"
      ></app-nutrition-metric>
      <app-nutrition-metric
        label="Carbs"
        [value]="carbs()"
        unit="g"
        colorClass="text-amber-400"
      ></app-nutrition-metric>
      <app-nutrition-metric
        label="Fat"
        [value]="fat()"
        unit="g"
        colorClass="text-rose-400"
      ></app-nutrition-metric>
      <app-nutrition-metric
        label="Fiber"
        [value]="fiber()"
        unit="g"
        colorClass="text-emerald-400"
        class="col-span-2 md:col-span-1"
      ></app-nutrition-metric>
    </div>
  `
})
export class NutritionSummaryComponent {
  energy = input<number>(2444);
  protein = input<number>(185);
  carbs = input<number>(242);
  fat = input<number>(76);
  fiber = input<number>(21);
}
