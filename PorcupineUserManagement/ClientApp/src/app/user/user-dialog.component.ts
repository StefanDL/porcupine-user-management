import {Component, Inject} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-dialog',
  template: `
    <h2 mat-dialog-title>@if (data) {
      Edit User
    } @else {
      Add New User
    }</h2>

    <mat-dialog-content [formGroup]="form" class="m-3">
      <mat-form-field appearance="outline" class="w-100 mb-3 mt-3">
        <mat-label>Name</mat-label>
        <input matInput formControlName="name" />
        <mat-error *ngIf="form.controls['name'].hasError('required')">Name is required</mat-error>
      </mat-form-field>

      <mat-form-field appearance="outline" class="w-100 mb-3">
        <mat-label>Email</mat-label>
        <input matInput formControlName="email" />
        <mat-error *ngIf="form.controls['email'].hasError('required')">Email is required</mat-error>
        <mat-error *ngIf="form.controls['email'].hasError('email')">Invalid email format</mat-error>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" [disabled]="form.invalid" (click)="save()">Save</button>
    </mat-dialog-actions>


  `,
  styles: ``,
  standalone: false
})
export class UserDialogComponent {
  form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UserDialogComponent>
  ) {

    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      id: ['']
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
