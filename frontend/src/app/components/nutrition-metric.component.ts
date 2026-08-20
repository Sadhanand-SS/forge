import { Component, input } from '@angular/core';

@Component({
  selector: 'app-nutrition-metric',
  standalone: true,
  template: `
    <div class="flex flex-col md:flex-row items-baseline md:gap-1.5 px-4 py-3 md:py-2 select-none">
      <span class="text-[0.625rem] font-bold text-zinc-500 tracking-widest uppercase font-mono mb-1 md:mb-0">
        {{ label() }}
      </span>
      <div class="flex items-baseline gap-0.5">
        <span class="text-sm font-bold tracking-tight" [class]="colorClass()">
          {{ value() }}
        </span>
        <span class="text-[0.7rem] font-medium text-zinc-500 lowercase font-mono">
          {{ unit() }}
        </span>
      </div>
    </div>
  `
})
export class NutritionMetricComponent {
  label = input.required<string>();
  value = input.required<number | string>();
  unit = input.required<string>();
  colorClass = input<string>('text-white');
}
