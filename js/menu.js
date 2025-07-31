 // Seleciona todos os links do menu
        const menuLinks = document.querySelectorAll('.menu__link');

        // Adiciona um evento de clique a cada link
        menuLinks.forEach(link => {
            link.addEventListener('click', function() {
                // Remove a classe 'ativo' de todos os links
                menuLinks.forEach(link => link.classList.remove('ativo'));
        
                // Adiciona a classe 'ativo' ao link clicado
                this.classList.add('ativo');
            });
        });

        // Verifica a URL atual e adiciona a classe 'ativo' ao item correspondente
        const currentPage = window.location.pathname;
        menuLinks.forEach(link => {
            if (link.href === window.location.href) {
                link.classList.add('ativo'); // Adiciona a classe 'ativo' se o link corresponder à página atual
            }
        });