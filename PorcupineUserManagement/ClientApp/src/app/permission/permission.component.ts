import {Component, inject, OnInit} from '@angular/core';
import {AppService} from "../app.service";
import {MatDialog} from "@angular/material/dialog";
import {PermissionDialogComponent} from "./permission-dialog.component";
import {HttpErrorResponse} from "@angular/common/module.d-CnjH8Dlt";

@Component({
  selector: 'app-permission',
  templateUrl: './permission.component.html',
  styleUrl: './permission.component.css',
  standalone: false
})
export class PermissionComponent implements OnInit{
  data: any[] = [];
  totalCount: number = 0;
  error: any;
  displayedColumns: string[] = ['name','actions'];
  selectedRow: any = null;

  onRowClick(row: any) {
    this.selectedRow = row;
  }
  constructor(private appService: AppService) {
  }
  readonly dialog = inject(MatDialog);

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, data:any): void {
    const dialogRef = this.dialog.open(PermissionDialogComponent, {
      width: '500px',
      enterAnimationDuration,
      exitAnimationDuration,
      data: data
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        if (result) {
          this.appService.createOrUpdateEntity('permission', result).subscribe(
            {
              next: (data) => {
                this.getPermissions();
              },
              error: (err: HttpErrorResponse) => {
                this.error = err;
              }
            }
          )
        }
      }
    });
  }
  ngOnInit(): void {
    this.getPermissions();
  }
  getPermissions(page: number = 1, pageSize: number = 10) {
    this.appService.getEntities('permission', page, pageSize).subscribe(
      {
        next: (data) => {
          if (data)
          {
            this.data = data.data;
            this.totalCount = data.total;
          }
          else {
            this.data = [];
          }

        },
        error: (err: HttpErrorResponse) => {
          this.data = [];
          this.error = err;
        }
      }
    )
  }

  addPermission() {
    this.openDialog('10ms', '10ms',null)
  }

  onPageChange($event: any) {
    this.getPermissions($event.pageIndex + 1, $event.pageSize);
  }

  editPermission(row: any) {
    this.openDialog('10ms', '10ms', row)
  }

  removePermission(row: any) {
    if (confirm('Are you sure you want to delete this permission?')) {
      this.appService.deleteEntity('permission', row.id).subscribe(
        {
          next: (data) => {
            this.getPermissions();
          },
          error: (err: HttpErrorResponse) => {
            this.error = err;
          }
        }
      )
    }
  }
}

