import { Component, input } from '@angular/core';

@Component({
  selector: 'app-entity-card',
  standalone: true,
  template: `
    <div class="p-6 rounded-2xl bg-[#17171a] border border-zinc-800 flex flex-col justify-between aspect-square md:aspect-[4/3] lg:aspect-[1.1] hover:border-zinc-700/60 shadow-sm transition-all select-none group cursor-pointer">
      <div>
        <!-- Number count -->
        <div class="text-4xl font-bold text-white tracking-tight leading-none mb-3">
          {{ count() }}
        </div>
        <!-- Title -->
        <h4 class="text-[0.925rem] font-bold text-white tracking-wide mb-0.5">
          {{ title() }}
        </h4>
        <!-- Description -->
        <p class="text-xs text-zinc-500 font-medium leading-relaxed">
          {{ description() }}
        </p>
      </div>

      <!-- Action link -->
      <div class="mt-4 flex items-center gap-1 text-[0.7rem] font-bold text-[#ea580c] group-hover:text-orange-500 tracking-widest uppercase font-mono transition-colors">
        <span>{{ actionText() }}</span>
        <span class="transform group-hover:translate-x-1.5 transition-transform duration-200">&rarr;</span>
      </div>
    </div>
  `
})
export class EntityCardComponent {
  count = input.required<number | string>();
  title = input.required<string>();
  description = input.required<string>();
  actionText = input<string>('Open');
}
