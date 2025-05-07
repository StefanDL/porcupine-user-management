import {Component, Inject} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AppService} from "../app.service";

@Component({
  selector: 'app-group-permission-add-dialog',
  standalone: false,
  template: `
    <h2 mat-dialog-title>Add Permission to Group</h2>

    <mat-dialog-content [formGroup]="form" class="m-3">
      <mat-form-field>
        <mat-label>Permission</mat-label>
        <mat-select formControlName="permission">
          @for (permission of permissions; track permission) {
            <mat-option  [value]="permission.id">{{permission.name}}</mat-option>
          }
        </mat-select>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: ``
})
export class GroupPermissionAddDialogComponent {
  permissions: any[] = [];
  form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public dataList: any[],
    private fb: FormBuilder,
    private appService : AppService,
    public dialogRef: MatDialogRef<GroupPermissionAddDialogComponent>
  ) {
    this.form = this.fb.group({
      permission: ['', Validators.required]
    });
    this.appService.getEntityCount('permission').subscribe(
      {
        next: (data: number) => {
          if (data)
          {
            this.appService.getEntities('permission',1,data).subscribe(
              {
                next: (data: any) => {
                  if (data)
                  {
                    this.permissions = data.data.filter((x: { id: any; }) => !this.dataList.some(y => y.id === x.id));
                  }
                  else {
                    this.permissions = [];
                  }
                }
              });
          }
        }
      }
    );
  }

  save(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value); // send data back
    }
  }
}
