import { Routes } from "@angular/router";

export const routes: Routes = [
  {
    path: "",
    loadComponent: () =>
      import("./features/catalog/catalog-page").then((m) => m.CatalogPage),
    title: "Catalogue · Newton Gaming Station",
  },
  {
    path: "games/new",
    loadComponent: () =>
      import("./features/game-edit/game-edit-page").then((m) => m.GameEditPage),
    title: "Add Game · Newton Gaming Station",
  },
  {
    path: "games/:id/edit",
    loadComponent: () =>
      import("./features/game-edit/game-edit-page").then((m) => m.GameEditPage),
    title: "Edit Game · Newton Gaming Station",
  },
  { path: "**", redirectTo: "" },
];
