import {Component, Inject} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-permission-dialog',
  standalone: false,
  template: `
    <h2 mat-dialog-title>
    @if (data) {
      Edit Permission
    } @else {
      Add New Permission
    }
    </h2>

    <mat-dialog-content [formGroup]="form" class="m-3">
      <mat-form-field appearance="outline" class="w-100 mb-3 mt-3">
        <mat-label>Name</mat-label>
        <input matInput formControlName="name" />
        <mat-error *ngIf="form.controls['name'].hasError('required')">Name is required</mat-error>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" [disabled]="form.invalid" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: ``
})
export class PermissionDialogComponent {
  form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PermissionDialogComponent>
  ) {

    this.form = this.fb.group({
      name: ['', Validators.required],
      id: [null]
    });
    if (data) {
      this.form.patchValue(data);
    }
  }

  save(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value); // send data back
    }
  }
}
