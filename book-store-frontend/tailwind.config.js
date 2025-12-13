/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#ecfdf5',
          500: '#208092',
          600: '#186978',
          700: '#145f6a',
        },
        accent: {
          500: '#32B8C6',
        }
      },
    },
  },
  plugins: [],
}
