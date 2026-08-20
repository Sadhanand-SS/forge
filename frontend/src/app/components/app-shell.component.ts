import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../components/sidebar.component';
import { PageHeaderComponent } from '../components/page-header.component';
import { DailyOutputCardComponent } from '../components/daily-output-card.component';
import { TodaysStructureComponent } from '../components/todays-structure.component';
import { DietEntitiesComponent } from '../components/diet-entities.component';

@Component({
  selector: 'app-app-shell',
  standalone: true,
  imports: [
    CommonModule,
    SidebarComponent,
    PageHeaderComponent,
    DailyOutputCardComponent,
    TodaysStructureComponent,
    DietEntitiesComponent
  ],
  template: `
    <div class="h-screen w-screen flex bg-[#101012] overflow-hidden font-sans text-zinc-100">
      <!-- Desktop Sidebar -->
      <div class="hidden lg:block h-full shrink-0">
        <app-sidebar></app-sidebar>
      </div>

      <!-- Mobile/Tablet Drawer Backdrop -->
      @if (isMobileSidebarOpen()) {
        <div
          class="lg:hidden fixed inset-0 z-40 bg-black/60 backdrop-blur-xs transition-opacity duration-200"
          (click)="toggleMobileSidebar()"
        ></div>
      }

      <!-- Mobile/Tablet Drawer Sidebar -->
      <div
        [class]="
          'lg:hidden fixed inset-y-0 left-0 z-50 transform transition-transform duration-300 ease-in-out shrink-0 ' +
          (isMobileSidebarOpen() ? 'translate-x-0' : '-translate-x-full')
        "
      >
        <app-sidebar></app-sidebar>
      </div>

      <!-- Main Content Area -->
      <div class="flex-1 flex flex-col h-full overflow-hidden">
        <!-- Top Bar for Mobile/Tablet -->
        <header class="lg:hidden flex items-center justify-between px-6 py-4 bg-[#0d0d0f] border-b border-zinc-800/80 shrink-0">
          <div class="flex items-center gap-3">
            <!-- Flame Icon logo -->
            <div class="flex items-center justify-center w-8 h-8 rounded-lg bg-[#18181c] border border-zinc-800">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4 h-4 text-[#ea580c]">
                <path d="M8.5 14.5A2.5 2.5 0 0 0 11 12c0-1.38-.5-2-1-3-1.072-2.143-.224-4.054 2-6 .5 2.5 2 4.9 4 6.5 2 1.6 3 3.5 3 5.5a7 7 0 1 1-14 0c0-1.153.433-2.294 1-3a2.5 2.5 0 0 0 2.5 2.5z" />
              </svg>
            </div>
            <span class="text-sm font-bold text-white tracking-widest uppercase font-mono">FORGE</span>
          </div>

          <button
            (click)="toggleMobileSidebar()"
            class="p-2 rounded-lg bg-zinc-800/40 text-zinc-300 hover:text-white border border-zinc-800/60 transition-all active:scale-95"
            aria-label="Toggle menu"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.8" stroke="currentColor" class="w-5 h-5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" />
            </svg>
          </button>
        </header>

        <!-- Page Content Scroll Area -->
        <main class="flex-1 overflow-y-auto px-6 py-8 md:px-10 md:py-10 bg-[#101012]">
          <div class="max-w-[1240px] mx-auto flex flex-col gap-6">
            <!-- Header section -->
            <app-page-header></app-page-header>

            <!-- Daily Output & Structure Section -->
            <div class="grid grid-cols-1 lg:grid-cols-5 gap-6">
              <!-- Left: Daily Output (60% on desktop) -->
              <div class="lg:col-span-3">
                <app-daily-output-card
                  [calories]="calories()"
                  [protein]="protein()"
                  [carbs]="carbs()"
                  [fat]="fat()"
                  [fiber]="fiber()"
                ></app-daily-output-card>
              </div>

              <!-- Right: Today's Structure (40% on desktop) -->
              <div class="lg:col-span-2">
                <app-todays-structure
                  [slots]="mealSlots()"
                ></app-todays-structure>
              </div>
            </div>

            <!-- Entities Grid Section -->
            <div class="mt-4">
              <app-diet-entities
                [entities]="entities()"
              ></app-diet-entities>
            </div>
          </div>
        </main>
      </div>
    </div>
  `
})
export class AppShellComponent implements OnInit {
  protected readonly isMobileSidebarOpen = signal(false);

  protected readonly calories = signal<number>(2444);
  protected readonly protein = signal<number>(185);
  protected readonly carbs = signal<number>(242);
  protected readonly fat = signal<number>(76);
  protected readonly fiber = signal<number>(21);

  protected readonly mealSlots = signal<Array<{ index: string; title: string; time: string }>>([
    { index: '01', title: 'Breakfast', time: '08:00' },
    { index: '02', title: 'Lunch', time: '13:00' },
    { index: '03', title: 'Dinner', time: '20:00' }
  ]);

  protected readonly entities = signal<Array<{ count: number | string; title: string; description: string }>>([
    { count: 8, title: 'Ingredients', description: 'Raw components' },
    { count: 4, title: 'Meal Items', description: 'Prepared foods' },
    { count: 3, title: 'Meals', description: 'Occasions' },
    { count: 3, title: 'Meals Per Day', description: 'Daily slots' },
    { count: 9, title: 'Units of Measure', description: 'Conversions' }
  ]);

  toggleMobileSidebar() {
    this.isMobileSidebarOpen.update(v => !v);
  }

  async ngOnInit() {
    const baseUrl = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1' ? 'http://localhost:5062' : '';
    
    // Get today's local date in YYYY-MM-DD
    const today = new Date().toLocaleDateString('sv');

    // 1. Fetch Daily Summary
    try {
      const summaryRes = await fetch(`${baseUrl}/api/DailyMeals/GetDailySummary?date=${today}`);
      if (summaryRes.ok) {
        const summary = await summaryRes.json();
        if (summary) {
          if (summary.totalNutrition) {
            this.calories.set(Math.round(summary.totalNutrition.calories || 0));
            this.protein.set(Math.round(summary.totalNutrition.protein || 0));
            this.carbs.set(Math.round(summary.totalNutrition.carbohydrates || 0));
            this.fat.set(Math.round(summary.totalNutrition.fat || 0));
            this.fiber.set(Math.round(summary.totalNutrition.fiber || 0));
          }
          if (summary.meals && summary.meals.length > 0) {
            const slots = summary.meals.map((m: any, i: number) => ({
              index: (i + 1).toString().padStart(2, '0'),
              title: m.mealName,
              time: m.time
            }));
            this.mealSlots.set(slots);
          }
          
          this.updateEntityCount('Meals Per Day', summary.meals?.length || summary.mealCount || 0);
        }
      }
    } catch (e) {
      console.warn("Failed to load daily summary, using fallback mock data:", e);
    }

    // 2. Fetch Ingredients Count
    try {
      const res = await fetch(`${baseUrl}/api/Ingredients/Get?limit=1000`);
      if (res.ok) {
        const data = await res.json();
        const count = data.items?.length || 0;
        this.updateEntityCount('Ingredients', count);
      }
    } catch (e) {
      console.warn("Failed to fetch ingredients count:", e);
    }

    // 3. Fetch Meal Items Count
    try {
      const res = await fetch(`${baseUrl}/api/MealItems/Get`);
      if (res.ok) {
        const data = await res.json();
        this.updateEntityCount('Meal Items', data.length || 0);
      }
    } catch (e) {
      console.warn("Failed to fetch meal items count:", e);
    }

    // 4. Fetch Meals Count
    try {
      const res = await fetch(`${baseUrl}/api/Meals/Get`);
      if (res.ok) {
        const data = await res.json();
        this.updateEntityCount('Meals', data.length || 0);
      }
    } catch (e) {
      console.warn("Failed to fetch meals count:", e);
    }

    // 5. Fetch Custom Units Count
    try {
      const res = await fetch(`${baseUrl}/api/CustomUnits/Get`);
      if (res.ok) {
        const data = await res.json();
        this.updateEntityCount('Units of Measure', data.length || 0);
      }
    } catch (e) {
      console.warn("Failed to fetch custom units count:", e);
    }
  }

  private updateEntityCount(title: string, count: number) {
    this.entities.update(list => 
      list.map(entity => 
        entity.title === title ? { ...entity, count } : entity
      )
    );
  }
}
