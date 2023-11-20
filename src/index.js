const BaseURL = 'https://localhost:44319/UserManager.asmx';
let users = [];
let selectedUser = {};
let selectedRowId = 0;
let deletedUserId = '';

document.addEventListener("DOMContentLoaded", function (event) {
    getUsers();

    function getUsers() {
        fetch(`${BaseURL}/GetUsers`, {
            method: 'GET',
            headers: {
                "Content-Type": "application/json charset=UTF-8"
            }
        })
            .then((response) => response.json())
            .then((response) => {
                addUsersToTable(response.Data);
                users = response.Data;
            });
    }
});

function addUser() {
    const user = getUserFromForm();

    fetch(`${BaseURL}/AddUser`, {
        method: 'POST',
        headers: {
            "Content-Type": "application/json charset=UTF-8"
        },
        body: JSON.stringify({
            firstName: user.FirstName,
            lastName: user.LastName,
            age: user.Age,
            email: user.Email
        }),
    })
        .then((response) => response.json())
        .then((response) => {
            const data = response.Data;
            users.push(data);
            addUserRow(data);
        });
}

function updateUser() {
    const user = getUserFromForm();

    fetch(`${BaseURL}/UpdateUser`, {
        method: 'POST',
        headers: {
            "Content-Type": "application/json charset=UTF-8"
        },
        body: JSON.stringify({
            id: user.Id,
            firstName: user.FirstName,
            lastName: user.LastName,
            age: user.Age,
            email: user.Email
        }),
    })
        .then((response) => response.json())
        .then((response) => {
            const data = response.Data;
            let user = users.find(user => user.Id === data.Id);

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Age = data.Age;
            user.Email = data.Email;

            updateUserRow(data);
        });
}

function deleteUser(rowId) {
    const deletedUser = users[rowId - 1];
    selectedRowId = rowId;
    deletedUserId = deletedUser.Id;
    
    fetch(`${BaseURL}/DeleteUser?id=${deletedUser.Id}`, {
        method: 'GET',
        headers: {
            "Content-Type": "application/json charset=UTF-8"
        },
    })
        .then((response) => response.json())
        .then(() => {
            users = users.filter(user => user.Id !== deletedUserId);
            deleteUserRow();
        });
}



//_____________________modal_________________________//

function openModalToEdit(rowId) {
    selectedRowId = rowId;

    const saveChangesBtn = document.getElementById('saveChanges');
    selectedUser = users[rowId - 1];

    fillFormContent(selectedUser);
    validateForm();

    saveChangesBtn.onclick = () => {
        if (!isValidForm()) {
            return;
        }

        updateUser();
        closeModal();
    }
}

function openModalToAdd() {
    clearForm();
    validateForm();

    const saveChangesBtn = document.getElementById('saveChanges');

    saveChangesBtn.onclick = (event) => {
        if (!isValidForm()) {
            return;
        }

        addUser();
        closeModal();
    }
}

function closeModal() {
    document.getElementById('closeBtn').click();
}



//_____________________table_________________________//

function addUsersToTable(users) {
    users.forEach((user) => {
        addUserRow(user);
    });
}

function addUserRow(user) {
    const elem = document.querySelector('table tbody');
    const template = document.getElementById('user-row');
    const clone = template.content.cloneNode(true);

    const  td = clone.querySelectorAll("td");
    const  th = clone.querySelector("th");

    th.textContent = elem.rows.length + 1;
    mapUserRow(user, td);

    elem.appendChild(clone);
}

function updateUserRow(user) {
    
    const row = document.querySelectorAll("tr")[selectedRowId];
    const td = row.querySelectorAll('td');
    mapUserRow(user, row);
}

function mapUserRow(user, td) {
    td[0].textContent = user.FirstName;
    td[1].textContent = user.LastName;
    td[2].textContent = user.Age;
    td[3].textContent = user.Email;
}

function deleteUserRow() {
    const row = document.querySelectorAll("tr")[selectedRowId];
    row.remove();
}



//_____________________form_________________________//
function fillFormContent(selectedUser) {
    inputs = document.querySelectorAll('.modal-dialog input');

    inputs[0].value = selectedUser.FirstName;
    inputs[1].value = selectedUser.LastName;
    inputs[2].value = selectedUser.Age;
    inputs[3].value = selectedUser.Email;
}

function clearForm() {
    const form = document.querySelector('.modal-dialog form');
    form.reset();
}

function validateForm() {
    inputs = document.querySelectorAll('.modal-dialog input');
    inputs.forEach(input => {
        validateInput(input);
    })
}

function validateInput(target) {
    if (target.validity.valid) {
        target.classList.remove("is-invalid");
        target.classList.add("is-valid");
    }
    else if (!target.validity.valid) {
        target.classList.remove("is-valid");
        target.classList.add("is-invalid");
    }
}

function getUserFromForm() {
    inputs = document.querySelectorAll('.modal-dialog input');

    return {
        Id: selectedUser?.Id,
        FirstName: inputs[0].value,
        LastName: inputs[1].value,
        Age: inputs[2].value,
        Email: inputs[3].value,
    };
}

function isValidForm() {
    return document.querySelector('.modal form').checkValidity();
}