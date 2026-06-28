import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";
import { NgbPagination } from "@ng-bootstrap/ng-bootstrap";

/**
 * MOLECULE: wraps ng-bootstrap's pagination so the rest of the app depends on our
 * own small surface (page + total) rather than the library directly.
 */
@Component({
  selector: "ngs-pagination",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgbPagination],
  templateUrl: "./pagination.html",
})
export class PaginationComponent {
  readonly page = input(1);
  readonly pageSize = input(12);
  readonly totalItems = input(0);
  readonly pageChange = output<number>();
}
