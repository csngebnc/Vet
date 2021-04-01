import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
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

  dataSource;
  displayedColumns: string[] = ['name', 'email', 'button'];

  users: VetUserDto[] = []

  ngOnInit(): void {

    this.filter = this.fb.group(
      {
        name: [''],
        email: ['']
      }
    )

    this.userService.getUsersFilter().subscribe((users: VetUserDto[]) => {
      this.users = users;
      this.dataSource = new MatTableDataSource<VetUserDto>(this.users);
    })
  }

  runFilter() {
    this.userService.getUsersFilter(this.filter.get('name').value, this.filter.get('email').value).subscribe((users: VetUserDto[]) => {
      this.users = users;
      this.dataSource = new MatTableDataSource<VetUserDto>(this.users);
    })
  }

}
