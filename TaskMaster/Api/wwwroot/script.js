async function onTelegramAuth(user) {
    // Telegram присылает данные в snake_case
    const camelCaseUser = {
        id: user.id,
        firstName: user.first_name,  // first_name → firstName
        lastName: user.last_name,   // last_name → lastName
        authDate: user.auth_date,   // auth_date → authDate
        username: user.username,
        photoUrl: user.photo_url,
        hash: user.hash
    };
    console.log(JSON.stringify(camelCaseUser));
    // Отправляем на бэкенд уже в camelCase
    // const result = await fetch('/auth/telegram', {
    //     method: 'POST',
    //     headers: {'Content-Type': 'application/json'},
    //     body: JSON.stringify(camelCaseUser)
    // });
    // if (result.ok) {
    //     const json = await result.json();
    //     console.log(json);
    // }
}