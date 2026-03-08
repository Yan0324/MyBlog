document.addEventListener('DOMContentLoaded', () => {
    // 1. Entrance Animations Sequence
    const elements = {
        logo: document.querySelector('.logo-section'),
        avatar: document.querySelector('.avatar-section'),
        status: document.querySelector('.status-section'),
        separator: document.querySelector('.separator'),
        quote: document.querySelector('.quote-section'),
        navItems: document.querySelectorAll('.nav-item')
    };

    // Helper to animate opacity and transform
    const animateIn = (el, delay, style = {}) => {
        setTimeout(() => {
            el.style.transition = 'all 0.8s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
            el.style.opacity = '1';
            el.style.transform = 'translate(0, 0) scale(1)';
            
            // Apply specific animations
            if (style.scale) el.style.transform = `scale(${style.scale})`;
            
            // Add visible class for CSS-based internal animations (like separator line)
            el.classList.add('visible');
        }, delay);
    };

    // Start Sequence
    setTimeout(() => {
        // Logo: Slide down
        animateIn(elements.logo, 100);
        
        // Avatar: Pop in (Elastic handled in CSS, here we trigger opacity/scale)
        setTimeout(() => {
            elements.avatar.style.transition = 'opacity 0.5s ease, transform 0.8s cubic-bezier(0.68, -0.55, 0.265, 1.55)';
            elements.avatar.style.opacity = '1';
            elements.avatar.style.transform = 'scale(1)';
        }, 300);

        // Status: Slide in from left
        animateIn(elements.status, 500);

        // Separator
        setTimeout(() => elements.separator.classList.add('visible'), 650);

        // Quote section fade in (Typewriter triggered separately)
        animateIn(elements.quote, 800);
        
        // Typewriter Effect
        setTimeout(startTypewriter, 800);

        // Nav Items: Staggered float up
        elements.navItems.forEach((item, index) => {
            animateIn(item, 1000 + (index * 100));
        });

    }, 100);

    // 2. Typewriter Effect
    function startTypewriter() {
        const textElement = document.getElementById('typewriter-text');
        const text = textElement.getAttribute('data-text');
        textElement.textContent = '';
        let i = 0;
        
        function type() {
            if (i < text.length) {
                textElement.textContent += text.charAt(i);
                i++;
                setTimeout(type, 100); // Typing speed
            } else {
                textElement.classList.add('done');
                // Show source after typing done
                document.querySelector('.quote-section').classList.add('visible');
            }
        }
        type();
    }

    // 3. 3D Tilt Effect for Avatar
    const avatarContainer = document.querySelector('.avatar-container');
    const avatarSection = document.querySelector('.avatar-section');

    if (!window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
        document.addEventListener('mousemove', (e) => {
            const rect = avatarContainer.getBoundingClientRect();
            const x = e.clientX - rect.left - rect.width / 2;
            const y = e.clientY - rect.top - rect.height / 2;
            
            // Only activate if mouse is relatively close (e.g., within window center area) or just small subtle movement globally
            // Let's make it follow mouse globally but with small dampening
            
            const rotateX = (y / window.innerHeight) * -10; // Max -5 to 5 deg
            const rotateY = (x / window.innerWidth) * 10;
            
            avatarContainer.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1)`;
        });

        // Reset on mouse leave (optional, but for global follow we might not need it, 
        // but let's add hover specific boost)
        avatarContainer.addEventListener('mouseenter', () => {
             avatarContainer.style.transition = 'transform 0.1s ease'; // Faster response on hover
        });
        
        avatarContainer.addEventListener('mouseleave', () => {
            avatarContainer.style.transition = 'transform 0.5s ease';
            avatarContainer.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)';
        });
    }

    // 4. Particle Canvas Background
    const canvas = document.getElementById('particle-canvas');
    const ctx = canvas.getContext('2d');
    
    let particles = [];
    const particleCount = 50; // Subtle amount
    
    function resizeCanvas() {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    }
    
    window.addEventListener('resize', resizeCanvas);
    resizeCanvas();
    
    class Particle {
        constructor() {
            this.x = Math.random() * canvas.width;
            this.y = Math.random() * canvas.height;
            this.size = Math.random() * 2 + 0.5;
            this.speedX = Math.random() * 0.5 - 0.25;
            this.speedY = Math.random() * 0.5 - 0.25;
            this.opacity = Math.random() * 0.5 + 0.1;
        }
        
        update() {
            this.x += this.speedX;
            this.y += this.speedY;
            
            // Wrap around screen
            if (this.x > canvas.width) this.x = 0;
            if (this.x < 0) this.x = canvas.width;
            if (this.y > canvas.height) this.y = 0;
            if (this.y < 0) this.y = canvas.height;
        }
        
        draw() {
            ctx.fillStyle = `rgba(100, 100, 100, ${this.opacity})`;
            ctx.beginPath();
            ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
            ctx.fill();
        }
    }
    
    function initParticles() {
        particles = [];
        for (let i = 0; i < particleCount; i++) {
            particles.push(new Particle());
        }
    }
    
    function animateParticles() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        for (let i = 0; i < particles.length; i++) {
            particles[i].update();
            particles[i].draw();
        }
        requestAnimationFrame(animateParticles);
    }
    
    // Check reduced motion before starting particles
    if (!window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
        initParticles();
        animateParticles();
    }

    // 5. Theme Toggle Logic (Preserved)
    const themeToggle = document.getElementById('theme-toggle');
    const body = document.body;
    const icon = themeToggle.querySelector('i');

    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        body.setAttribute('data-theme', savedTheme);
        updateIcon(savedTheme);
    }

    themeToggle.addEventListener('click', () => {
        const currentTheme = body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        body.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        updateIcon(newTheme);
    });

    function updateIcon(theme) {
        if (theme === 'dark') {
            icon.classList.remove('fa-moon');
            icon.classList.add('fa-sun');
        } else {
            icon.classList.remove('fa-sun');
            icon.classList.add('fa-moon');
        }
    }
});
