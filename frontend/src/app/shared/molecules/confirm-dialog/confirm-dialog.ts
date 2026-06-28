import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ButtonComponent } from "@shared/atoms/button/button";

/**
 * MOLECULE: a reusable confirmation modal rendered via NgbModal. It is instantiated
 * imperatively (modal.open), so it exposes plain properties rather than signal inputs
 * — the opener assigns them on componentInstance. Resolves true on confirm.
 */
@Component({
  selector: "ngs-confirm-dialog",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ButtonComponent],
  templateUrl: "./confirm-dialog.html",
})
export class ConfirmDialogComponent {
  readonly modal = inject(NgbActiveModal);

  title = "Please confirm";
  message = "Are you sure?";
  confirmLabel = "Confirm";
}
