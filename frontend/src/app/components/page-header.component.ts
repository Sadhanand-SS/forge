import { Component } from '@angular/core';

@Component({
  selector: 'app-page-header',
  standalone: true,
  template: `
    <header class="mb-7 select-none">
      <!-- Breadcrumb -->
      <div class="flex items-center gap-1.5 text-[0.6875rem] font-bold tracking-widest text-zinc-500 uppercase font-mono mb-4">
        <span class="hover:text-zinc-300 cursor-pointer transition-colors">Forge</span>
        <span class="text-zinc-700">/</span>
        <span class="text-zinc-300">Dashboard</span>
      </div>

      <!-- Title & Subtitle -->
      <h1 class="text-3xl font-bold text-white tracking-tight leading-none mb-2">
        Dashboard
      </h1>
      <p class="text-sm text-zinc-400 font-medium">
        System overview of your diet configuration
      </p>
    </header>
  `
})
export class PageHeaderComponent {}
