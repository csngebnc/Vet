import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PagedList } from 'src/app/_models/PagedList';
import { VetUserDto } from 'src/app/_models/vetuserdto';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['./list-users.component.css']
})
export class ListUsersComponent implements OnInit {

  constructor(private userService: UserService, private fb: FormBuilder) { }

  filter: FormGroup

  dataSource = new MatTableDataSource<VetUserDto>();
  displayedColumns: string[] = ['name', 'email', 'button'];

  pageEvent: PageEvent;
  lenght: number;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  users: VetUserDto[] = []

  ngOnInit(): void {

    this.filter = this.fb.group(
      {
        name: [''],
        email: ['']
      }
    )
    this.dataSource.paginator = this.paginator;
    this.runFilter();
  }

  runFilter() {
    if (this.users.length > 0)
      this.paginator.firstPage();
    this.pageChanged({ pageIndex: 0, pageSize: 10, length: 0 });
  }

  pageChanged(event: PageEvent) {
    this.userService.getUsersFilter(event, this.filter.get('name').value, this.filter.get('email').value).subscribe((users: PagedList<VetUserDto>) => {
      this.users = users.items;
      this.dataSource.data = users.items;
      this.lenght = users.total;
    })
    return event;
  }

}
