import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RouterLink, RouterOutlet } from "@angular/router";

@Component({
  selector: "ngs-root",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, RouterLink],
  template: `
    <nav class="navbar navbar-dark bg-dark mb-4">
      <div class="container">
        <a class="navbar-brand fw-bold" routerLink="/"
          >🎮 Newton Gaming Station</a
        >
        <a class="btn btn-success btn-sm" routerLink="/games/new">+ Add Game</a>
      </div>
    </nav>
    <main class="container pb-5">
      <router-outlet />
    </main>
  `,
})
export class App {}
