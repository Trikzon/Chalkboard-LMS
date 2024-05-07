create schema lms;

create table courses
(
    course_id   char(36)     not null
        primary key,
    name        varchar(255) not null,
    code        varchar(255) not null,
    description text         null
);

create table modules
(
    module_id char(36)     not null
        primary key,
    course_id char(36)     not null,
    name      varchar(255) not null,
    constraint modules_courses_course_id_fk
        foreign key (course_id) references courses (course_id)
);

create table content_items
(
    content_item_id char(36)     not null
        primary key,
    module_id       char(36)     not null,
    name            varchar(255) not null,
    content         text         null,
    constraint content_items_modules_module_id_fk
        foreign key (module_id) references modules (module_id)
);

create table assignments
(
    content_item_id        char(36) not null
        primary key,
    total_available_points int      not null,
    due_date               datetime not null,
    constraint assignments_content_items_content_item_id_fk
        foreign key (content_item_id) references content_items (content_item_id)
);

create table students
(
    student_id     char(36)                                                       not null
        primary key,
    name           varchar(255)                                                   not null,
    classification enum ('Freshman', 'Sophomore', 'Junior', 'Senior', 'Graduate') not null
);

create table enrollments
(
    course_id  char(36) not null,
    student_id char(36) not null,
    primary key (course_id, student_id),
    constraint enrollments_courses_course_id_fk
        foreign key (course_id) references courses (course_id),
    constraint enrollments_students_student_id_fk
        foreign key (student_id) references students (student_id)
);

create table submissions
(
    content_item_id char(36) not null,
    student_id      char(36) not null,
    content         text     null,
    submission_date datetime not null,
    points          decimal  not null,
    primary key (content_item_id, student_id),
    constraint submissions_assignments_content_item_id_fk
        foreign key (content_item_id) references assignments (content_item_id),
    constraint submissions_students_student_id_fk
        foreign key (student_id) references students (student_id)
);